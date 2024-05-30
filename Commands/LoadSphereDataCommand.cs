using ODSphereRouter.Stores;
using ODSphereRouter.ViewModels;
using ODUtils.Commands;

namespace ODSphereRouter.Commands
{
    public sealed class LoadSphereDataCommand(RoutePlannerViewModel routePlannerViewModel, SphereDataStore sphereDataStore) : AsyncCommandBase
    {
        private readonly RoutePlannerViewModel routePlannerViewModel = routePlannerViewModel;
        private readonly SphereDataStore sphereDataStore = sphereDataStore;

        public override async Task ExecuteAsync(object? parameter)
        {
            routePlannerViewModel.IsLoading = true;

            try
            {
                await sphereDataStore.Load();

                routePlannerViewModel.UpdateLists(sphereDataStore);
            }
            catch (Exception) 
            { 

            }

            routePlannerViewModel.IsLoading = false;
        }
    }
}