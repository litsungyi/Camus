namespace Camus.UiUtilities
{
    public interface IButtonProcessingHandler
    {
        bool IsProcessing
        {
            get;
            set;
        }
    }

    public static class ButtonProcessingHandlerExtension
    {
        public static void BeginProcessing(this IButtonProcessingHandler handler)
        {
            handler.IsProcessing = true;
        }

        public static void EndProcessing(this IButtonProcessingHandler handler)
        {
            handler.IsProcessing = false;
        }
    }
}
