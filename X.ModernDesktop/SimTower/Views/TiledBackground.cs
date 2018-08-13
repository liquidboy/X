using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace X.ModernDesktop.SimTower.Views
{
  /// <summary>
  /// Modified from solution which described in http://www.codeproject.com/Tips/777534/Tiled-Background-for-Windows-Store-Applications
  /// </summary>
  public class TiledBackground : Panel
  {
    public ImageSource BackgroundImage
    {
      get { return (ImageSource)GetValue(BackgroundImageProperty); }
      set { SetValue(BackgroundImageProperty, value); }
    }

    // Using a DependencyProperty as the backing store for BackgroundImage.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty BackgroundImageProperty =
        DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(TiledBackground), new PropertyMetadata(null, BackgroundImageChanged));


    private static void BackgroundImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((TiledBackground)d).OnBackgroundImageChanged();
    }
    private static void DesignDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((TiledBackground)d).OnDesignDataChanged();
    }

    private ImageBrush backgroundImageBrush = null;

    private bool tileImageDataRebuildNeeded = true;
    private byte[] tileImagePixels = null;
    private int tileImageWidth = 0;
    private int tileImageHeight = 0;

    private readonly BitmapPixelFormat bitmapPixelFormat = BitmapPixelFormat.Bgra8;
    private readonly BitmapTransform bitmapTransform = new BitmapTransform();
    private readonly BitmapAlphaMode bitmapAlphaMode = BitmapAlphaMode.Straight;
    private readonly ExifOrientationMode exifOrientationMode = ExifOrientationMode.IgnoreExifOrientation;
    private readonly ColorManagementMode coloManagementMode = ColorManagementMode.ColorManageToSRgb;

    public TiledBackground()
    {
      this.backgroundImageBrush = new ImageBrush();
      this.Background = backgroundImageBrush;

      this.SizeChanged += TiledBackground_SizeChanged;
    }

    private async void TiledBackground_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      await this.Render((int)e.NewSize.Width, (int)e.NewSize.Height);
    }

    private async void OnBackgroundImageChanged()
    {
      tileImageDataRebuildNeeded = true;
      await Render((int)this.ActualWidth, (int)this.ActualHeight);
    }

    private async void OnDesignDataChanged()
    {
      tileImageDataRebuildNeeded = true;
      await Render((int)this.ActualWidth, (int)this.ActualHeight);
    }

    private async Task RebuildTileImageData()
    {
      BitmapImage image = BackgroundImage as BitmapImage;
      if ((image != null) && (!DesignMode.DesignModeEnabled))
      {
        string imgUri = image.UriSource.OriginalString;
        if (!imgUri.Contains("ms-appx:///"))
        {
          imgUri += "ms-appx:///";
        }
        var imageSource = new Uri(imgUri);
        StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(imageSource);
        using (var imageStream = await storageFile.OpenAsync(FileAccessMode.Read))
        {
          BitmapDecoder decoder = await BitmapDecoder.CreateAsync(imageStream);

          var pixelDataProvider = await decoder.GetPixelDataAsync(this.bitmapPixelFormat, this.bitmapAlphaMode,
              this.bitmapTransform, this.exifOrientationMode, this.coloManagementMode
              );

          this.tileImagePixels = pixelDataProvider.DetachPixelData();
          this.tileImageHeight = (int)decoder.PixelHeight;
          this.tileImageWidth = (int)decoder.PixelWidth;
        }
      }
    }

    private byte[] CreateBackgroud(int width, int height)
    {
      int bytesPerPixel = this.tileImagePixels.Length / (this.tileImageWidth * this.tileImageHeight);
      byte[] data = new byte[width * height * bytesPerPixel];

      int y = 0;
      int fullTileInRowCount = width / tileImageWidth;
      int tileRowLength = tileImageWidth * bytesPerPixel;

      //Stage 1: Go line by line and create a block of our pattern
      //Stop when tile image height or required height is reached
      while ((y < height) && (y < tileImageHeight))
      {
        int tileIndex = y * tileImageWidth * bytesPerPixel;
        int dataIndex = y * width * bytesPerPixel;

        //Copy the whole line from tile at once
        for (int i = 0; i < fullTileInRowCount; i++)
        {
          Array.Copy(tileImagePixels, tileIndex, data, dataIndex, tileRowLength);
          dataIndex += tileRowLength;
        }

        //Copy the rest - if there is any
        //Length will evaluate to 0 if all lines were copied without remainder
        Array.Copy(tileImagePixels, tileIndex, data, dataIndex,
                   (width - fullTileInRowCount * tileImageWidth) * bytesPerPixel);
        y++; //Next line
      }

      //Stage 2: Now let's copy those whole blocks from top to bottom
      //If there is not enough space to copy the whole block, skip to stage 3
      int rowLength = width * bytesPerPixel;
      int blockLength = this.tileImageHeight * rowLength;

      while (y <= (height - tileImageHeight))
      {
        int dataBaseIndex = y * width * bytesPerPixel;
        Array.Copy(data, 0, data, dataBaseIndex, blockLength);
        y += tileImageHeight;
      }

      //Copy the rest line by line
      //Use previous lines as source
      for (int row = y; row < height; row++)
        Array.Copy(data, (row - tileImageHeight) * rowLength, data, row * rowLength, rowLength);

      return data;
    }

    private async Task Render(int width, int height)
    {
      Stopwatch fullsw = Stopwatch.StartNew();

      if (tileImageDataRebuildNeeded)
        await RebuildTileImageData();

      if ((height > 0) && (width > 0))
      {
        using (var randomAccessStream = new InMemoryRandomAccessStream())
        {
          Stopwatch sw = Stopwatch.StartNew();
          var backgroundPixels = CreateBackgroud(width, height);
          sw.Stop();
          Debug.WriteLine("Background generation finished: {0} ticks - {1} ms", sw.ElapsedTicks, sw.ElapsedMilliseconds);

          BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, randomAccessStream);
          encoder.SetPixelData(this.bitmapPixelFormat, this.bitmapAlphaMode, (uint)width, (uint)height, 96, 96, backgroundPixels);
          await encoder.FlushAsync();

          if (this.backgroundImageBrush.ImageSource == null)
          {
            BitmapImage bitmapImage = new BitmapImage();
            randomAccessStream.Seek(0);
            bitmapImage.SetSource(randomAccessStream);
            this.backgroundImageBrush.ImageSource = bitmapImage;
          }
          else ((BitmapImage)this.backgroundImageBrush.ImageSource).SetSource(randomAccessStream);
        }
      }
      else this.backgroundImageBrush.ImageSource = null;

      fullsw.Stop();
      Debug.WriteLine("Background rendering finished: {0} ticks - {1} ms", fullsw.ElapsedTicks, fullsw.ElapsedMilliseconds);
    }
  }
}
