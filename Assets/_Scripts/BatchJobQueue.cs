using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace HitAndRun
{
    public class BatchJobQueue<T>
    {
        private readonly ConcurrentQueue<T> _queue = new();
        private readonly TimeSpan _batchDelay;
        private readonly int _batchSize;
        private readonly CancellationTokenSource _cts = new();
        private volatile bool _isProcessing;
        private readonly Func<List<T>, CancellationToken, UniTask> _processBatchAsync;

        public BatchJobQueue(TimeSpan batchDelay, int batchSize,
            Func<List<T>, CancellationToken, UniTask> processBatchAsync)
        {
            _batchDelay = batchDelay;
            _batchSize = batchSize;
            _processBatchAsync = processBatchAsync;
        }

        public void Enqueue(T job)
        {
            _queue.Enqueue(job);
            if (!_isProcessing)
                ProcessQueueAsync(_cts.Token).Forget();
        }

        private async UniTaskVoid ProcessQueueAsync(CancellationToken cancellationToken)
        {
            try
            {
                _isProcessing = true;
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (_queue.IsEmpty)
                    {
                        _isProcessing = false;
                        return;
                    }
                    else if (_queue.Count < _batchSize)
                    {
                        await UniTask.Delay(_batchDelay, cancellationToken: cancellationToken);
                    }
                    var batch = new List<T>(_batchSize);
                    while (batch.Count < _batchSize && _queue.TryDequeue(out var job))
                    {
                        batch.Add(job);
                    }

                    if (batch.Count > 0)
                    {
                        await _processBatchAsync(batch, cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Debug.Log("Batch processing stopped.");
            }
        }

        public void Clear()
        {
            _cts.Cancel();
            _isProcessing = false;
        }
    }

}