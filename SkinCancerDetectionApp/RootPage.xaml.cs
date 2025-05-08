using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Maui.Controls;

namespace SkinCancerDetectionApp;

public partial class RootPage : ContentPage
{
    public RootPage()
    {
        InitializeComponent();

        BlazorWebView.BlazorWebViewInitializing += BlazorWebViewInitializing;
        BlazorWebView.BlazorWebViewInitialized += BlazorWebViewInitialized;
    }

    private partial void BlazorWebViewInitializing(object? sender, BlazorWebViewInitializingEventArgs e);
    private partial void BlazorWebViewInitialized(object? sender, BlazorWebViewInitializedEventArgs e);
}