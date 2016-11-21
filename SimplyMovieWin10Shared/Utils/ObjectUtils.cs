using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SimplyMovieWin10Shared.Utils
{
    public static class ObjectUtils
    {

        /// <summary>
        /// Converti un fichier en byte[]
        /// </summary>
        /// <param name="file">le fichier à convertir</param>
        /// <returns>le byte[]</returns>
        public static async Task<byte[]> ConvertFileToBytes(StorageFile file)
        {
            using (var inputStream = await file.OpenSequentialReadAsync())
            {
                var readStream = inputStream.AsStreamForRead();

                var byteArray = new byte[readStream.Length];
                await readStream.ReadAsync(byteArray, 0, byteArray.Length);
                return byteArray;
            }
        }

        /// <param name="tab">le tableau de bytes</param>
        /// <returns>l'image</returns>
        public static async Task<BitmapImage> ConvertBytesToBitmap(byte[] tab)
        {
            using (var stream = new InMemoryRandomAccessStream())
            {
               using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
               {
                  writer.WriteBytes(tab);
                  await writer.StoreAsync();
               }
                var image = new BitmapImage();
                await image.SetSourceAsync(stream);
                return image;
             }
        }

        /// <summary>
        /// Rédimmenssionne une image
        /// </summary>
        /// <param name="sourceImage">l'image source</param>
        /// <param name="maxWidth">la taille max largeur</param>
        /// <param name="maxHeight">la taille max hauteur</param>
        /// <returns>l'image re dimensionnée</returns>
        public static BitmapImage ResizedImage(BitmapImage sourceImage, int maxWidth, int maxHeight)
        {
            var origHeight = sourceImage.PixelHeight;
            var origWidth = sourceImage.PixelWidth;
            var ratioX = maxWidth / (float)origWidth;
            var ratioY = maxHeight / (float)origHeight;
            var ratio = Math.Min(ratioX, ratioY);
            var newHeight = (int)(origHeight * ratio);
            var newWidth = (int)(origWidth * ratio);

            sourceImage.DecodePixelWidth = newWidth;
            sourceImage.DecodePixelHeight = newHeight;

            return sourceImage;
        }
    }
}
