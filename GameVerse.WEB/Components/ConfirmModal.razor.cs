using Microsoft.AspNetCore.Components;

namespace GameVerse.WEB.Components
{
    public partial class ConfirmModal : ComponentBase
    {
        [Parameter] public bool IsVisible { get; set; }
        [Parameter] public string Title { get; set; } = "Confirmer l'action";
        [Parameter] public string Message { get; set; } = "Es-tu sûre de vouloir continuer ?";
        [Parameter] public string ConfirmLabel { get; set; } = "Confirmer";

        [Parameter] public EventCallback OnConfirm { get; set; }
        [Parameter] public EventCallback OnCancel { get; set; }

        private async Task Confirm()
        {
            await OnConfirm.InvokeAsync();
        }

        private async Task Cancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
