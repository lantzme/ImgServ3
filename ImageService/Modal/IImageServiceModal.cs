﻿namespace ImageService.Modal
{
    public interface IImageServiceModal
    {
        /// <summary>
        /// The Function Addes a file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <param name="result"></param>
        /// <returns>Indication if the Addition Was Successful</returns>
        string AddFile(string path, out bool result);
    }
}
