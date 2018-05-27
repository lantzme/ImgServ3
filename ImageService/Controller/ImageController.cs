using System.Collections.Generic;
using System.Threading.Tasks;
using ImageService.Commands;
using ImageService.Configuration;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        #region Members
        protected Dictionary<int, ICommand> CommandsDict;
        protected IImageServiceModal Modal;
        protected readonly ILoggingService Logging;
        #endregion

        #region C'tor

        /// <summary>
        /// A c'tor for an ImageController.
        /// </summary>
        /// <param name="modalParameters">An IModalParameters object to work with</param>
        /// <param name="loggingService"></param>
        public ImageController(IModalParameters modalParameters, ILoggingService loggingService)
        {
            Logging = loggingService;
            CreateImageModal(modalParameters);

            // Create the command dictionary which will be used:
            CreateDictionary();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates an IImageServiceModal object instance based on the params.
        /// </summary>
        /// <param name="modalParameters">IModalParameters object</param>
        private void CreateImageModal(IModalParameters modalParameters)
        {
            Modal = new ImageServiceModal(modalParameters.OutputDir, modalParameters.ThumbnailSize);
        }

        /// <summary>
        /// Create a commands dictionary.
        /// </summary>
        private void CreateDictionary()
        {
            CommandsDict = new Dictionary<int, ICommand>
            {
                {(int) CommandEnum.NewFileCommand, new NewFileCommand(Modal)},
            };
        }

        /// <summary>
        /// Execute the command by sending the ID to ExecuteCommandById.
        /// </summary>
        /// <param name="commandId">The command ID</param>
        /// <param name="args">The recieved args</param>
        /// <param name="resultSuccesful">A result boolean</param>
        /// <param name="responseBack"></param>
        public string ExecuteCommand(int commandId, string[] args, out bool resultSuccesful, out bool responseBack)
        {
            Task<CommandResult> task = new Task<CommandResult>(() => ExecuteCommandById(commandId, args));
            task.Start();
            task.Wait();
            CommandResult result = task.Result;
            resultSuccesful = result.IsExecutedSuccessfully;
            responseBack = result.ResponseBack;
            return result.Messege;
        }

        /// <summary>
        /// Execute the input command by the given ID.
        /// </summary>
        /// <param name="commandId">Command id to execute</param>
        /// <param name="args">Command arguments</param>
        /// <returns>CommandResult object</returns>
        protected CommandResult ExecuteCommandById(int commandId, string[] args)
        {
            //Logging.Log($"Executing Command {commandId}", MessageTypeEnum.INFO);
            bool executionResult, responseBack;
            string message = CommandsDict[commandId].Execute(args, out executionResult, out responseBack);
            return new CommandResult()
            {
                Messege = message,
                IsExecutedSuccessfully = executionResult,
                ResponseBack = responseBack
            };
        }
        #endregion
    }
}
