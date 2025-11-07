using Timer = System.Timers.Timer;

namespace Masasamjant.Auditing.Abstractions
{
    /// <summary>
    /// Represents abstract auditor that provides functionality to audit application actions.
    /// </summary>
    public abstract class Auditor : IAuditor, IAuditingEventSource, IDisposable
    {
        private readonly Lock lockSources = new Lock();
        private readonly List<IProvideAuditingEvent> sources = new List<IProvideAuditingEvent>();
        private readonly object lockQueue = new object();
        private Queue<AuditingEventArgs> queue = new Queue<AuditingEventArgs>();
        private readonly Timer timer = new Timer();

        /// <summary>
        /// Initialzes a new instance of the <see cref="Auditor"/> class with specified configuration.
        /// </summary>
        /// <param name="configuration">The <see cref="AuditorConfiguration"/>.</param>
        protected Auditor(AuditorConfiguration configuration)
        {
            Configuration = configuration;
            IsDisposed = false;
        }
       
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        protected AuditorConfiguration Configuration { get; }

        /// <summary>
        /// Gets whether or not instance is disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// Appends specified <see cref="IProvideAuditingEvent"/> to auditor.
        /// </summary>
        /// <param name="source">The <see cref="IProvideAuditingEvent"/> to append.</param>
        public void Append(IProvideAuditingEvent source)
        {
            if (IsDisposed)
                return;

            lock (lockSources) 
            {
                if (Contains(source))
                    return;

                sources.Add(source);
                source.Audit += OnAuditAsync;
            }
        }

        /// <summary>
        /// Check if specified object is audited.
        /// </summary>
        /// <param name="instance">The object instance.</param>
        /// <returns><c>true</c> if <paramref name="instance"/> is audited; <c>false</c> otherwise.</returns>
        public virtual Task<bool> IsAuditedAsync(object instance)
        {
            var excluded = Configuration.ExcludedTypes.Contains(GetType().FullName);

            return Task.FromResult(!excluded);
        }

        /// <summary>
        /// Removes specified <see cref="IProvideAuditingEvent"/> from auditor.
        /// </summary>
        /// <param name="source">The <see cref="IProvideAuditingEvent"/> to remove.</param>
        public void Remove(IProvideAuditingEvent source)
        {
            if (IsDisposed)
                return;

            lock (lockSources)
            {
                if (sources.Remove(source))
                    source.Audit -= OnAuditAsync;
            }
        }

        /// <summary>
        /// Enables auditor to store auditing events.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public void Enable()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (timer.Enabled)
                return;

            timer.AutoReset = true;
            timer.Interval = Configuration.AuditingTimeoutMilliseconds;
            timer.Elapsed += OnTimerElapsed;
            timer.Start();
        }

        /// <summary>
        /// Disables auditor from storing auditing events.
        /// </summary>
        /// <exception cref="ObjectDisposedException">If instance is disposed.</exception>
        public void Disable()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().FullName);

            if (timer.Enabled)
            {
                timer.Stop();
                timer.Elapsed -= OnTimerElapsed;
            }
        }

        /// <summary>
        /// Tries to find auditing event with specified identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>A found auditing event or <c>null</c></returns>
        public abstract Task<AuditingEvent?> FindEventAsync(Guid identifier);

        /// <summary>
        /// Gets auditing events based on the specified search request.
        /// </summary>
        /// <param name="request">The <see cref="AuditingEventSearchRequest"/>.</param>
        /// <returns>A found auditing events.</returns>
        public abstract Task<IEnumerable<AuditingEvent>> SearchEventsAsync(AuditingEventSearchRequest request);

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes current instance.
        /// </summary>
        /// <param name="disposing"><c>true</c> if disposing; <c>false</c> otherwise.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            if (timer.Enabled)
            {
                timer.Stop();
                timer.Elapsed -= OnTimerElapsed;
            }

            timer.Dispose();

            var task = OnAuditingAsync();
            task.Wait();

            lock (lockSources)
            {
                foreach (var source in sources)
                    source.Audit -= OnAuditAsync;
                sources.Clear();
            }
        }

        /// <summary>
        /// Derived classes must override to audit specified <see cref="AuditingEvent"/> asynchronously.
        /// </summary>
        /// <param name="auditingEvent">The <see cref="AuditingEvent"/>.</param>
        /// <returns>A task.</returns>
        protected abstract Task OnAuditingAsync(AuditingEvent auditingEvent);

        private bool Contains(IProvideAuditingEvent source)
        {
            return sources.Any(s => ReferenceEquals(s, source));
        }

        private Task OnAuditAsync(object? sender, AuditingEventArgs e)
        {
            return Task.Factory.StartNew(() => 
            {
                if (IsDisposed || !timer.Enabled)
                    return;
                
                lock (lockQueue)
                {
                    queue.Enqueue(e);
                }
            });
        }

        private async void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsDisposed)
                return;

            await OnAuditingAsync();
        }

        private async Task OnAuditingAsync()
        {
            Queue<AuditingEventArgs> items;

            // Take current queue.
            lock (lockQueue)
            {
                if (queue.Count == 0)
                    return;

                items = queue;
                queue = new Queue<AuditingEventArgs>();
            }

            // Audit each item in the queue.
            while (items.TryDequeue(out var item))
            {
                await OnAuditingAsync(item.AuditingEvent);
            }
        }
    }
}
