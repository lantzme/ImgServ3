using ImageService.Modal;

namespace ImageService.Commands
{
    public class GetConfigCommand : ICommand
    {
        #region Members
        private readonly IImageServiceModal _modal;
        #endregion

        #region C'tor
        /// <summary>
        /// A c'tor for a NewFileCommand
        /// </summary>
        /// <param name="modal">The modal which will be used.</param>
        public GetConfigCommand(IImageServiceModal modal)
        {
            _modal = modal;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="args">Command arguments</param>
        /// <param name="result">Result variable</param>
        /// <returns>A result string</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;
            return "placeholder";
        }
        #endregion
    }
}
