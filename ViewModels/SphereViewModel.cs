using ODSphereRouter.Models;
using ODSphereRouter.Stores;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ODSphereRouter.ViewModels
{
    public class SphereViewModel : ViewModelBase
    {
        private readonly Sphere _sphere;
        private readonly SphereDataStore sphereDataStore;
        private readonly ObservableCollection<SphereSystemViewModel> _systems;

        public string ControllingSystem => _sphere.ControllingSystem;
        public IEnumerable<SphereSystemViewModel> Systems => _systems;


        public SphereViewModel(Sphere sphere, SphereDataStore sphereDataStore)
        {
            _sphere = sphere;
            this.sphereDataStore = sphereDataStore;
            _systems = [];

            _systems.CollectionChanged += OnSystemsCollectionChanged;

            foreach (var system in sphere.Systems)
            {
                var knownSystem = sphereDataStore.GetSystem(system);

                if (knownSystem != null)
                {
                    _systems.Add(new(knownSystem));
                }
            }
        }

        private void OnSystemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Systems));
        }
    }
}