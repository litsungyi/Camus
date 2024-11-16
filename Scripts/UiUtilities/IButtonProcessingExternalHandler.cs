namespace Camus.UiUtilities
{
    public interface IButtonProcessingExternalHandler
    {
        IButtonProcessingHandler Holder
        {
            get;
            set;
        }
    }

    public static class ButtonProcessingExternalHandlerExtension
    {
        public static void SetHandler(this IButtonProcessingExternalHandler handler, IButtonProcessingHandler holder)
        {
            handler.Holder = holder;
        }
    }
}
