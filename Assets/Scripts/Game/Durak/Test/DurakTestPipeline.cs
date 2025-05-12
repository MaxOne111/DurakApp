using System;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using Zenject;

namespace Game.Durak.Test
{
    public sealed class DurakTestPipeline
        : IInitializable, IDisposable
    {
        public DurakTestPipeline(TestConfigurationScreen configurationScreen, TestRoomScreen roomScreen)
        {
            _configurationScreen = configurationScreen;
            _roomScreen = roomScreen;
        }

        private readonly TestConfigurationScreen _configurationScreen;
        private readonly TestRoomScreen _roomScreen;

        private CancellationTokenSource _source;
        
        public async void Initialize()
        {
            _source = new CancellationTokenSource();
            await Start();
        }

        public void Dispose()
        {
            _source.Dispose();
        }

        private async Task Start()
        {
            var context = SynchronizationContext.Current;
            
            // Конфигурируем лобби
            _configurationScreen.gameObject.SetActive(true);
            var configuration = await _configurationScreen.WaitForApply();
            _configurationScreen.gameObject.SetActive(false);
            
            // Играем
            _roomScreen.gameObject.SetActive(true);
            var source = new TaskCompletionSource<bool>();
            context.Post(Callback, null);
            await source.Task;
            _roomScreen.gameObject.SetActive(false);
            
            // TODO:
            // Получать из предыдущего асинхронного метода информацию о победивших игроках, далее - показывать статистику
            // и разделять выигрыш (и т.д.)

            async void Callback(object state)
            {
                await _roomScreen.Play(configuration);
                source.SetResult(true);
            }
        }
    }
}