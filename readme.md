# Xamarin.Forms.Platform.AvaloniaUI

## Origins
This project was originally based on [this](https://github.com/zhongzf/Xamarin.Forms.Platform.Avalonia) original code, but it has been reordered, revised and extended to make it work with modern [AvaloniaUI](https://avaloniaui.net/).

## Project aim
This project is intended to explore the possibility of using Avalonia as a platform to keep the [Xamarin Forms](https://github.com/xamarin/Xamarin.Forms) codebase working in to the future. By using Avalonia UI we remove the need to write teh same code for multiple platforms. Avalonia already supports Windows, MacOS, iOS, Android and Linux from one codebase. This project ais to get as much of the original Xamarin codebase working with Avalonia as is feasable, and to provide a Proof of Concept platform for others to try to port their legacy Xamarin apps to.

## Why?
Whilst [Maui](https://dotnet.microsoft.com/en-us/apps/maui) does work, the Xamarin Forms to Maui transition is non as trivial as some would imply. We hope in this project to explore an alternate idea. The reality is, this is not a professional project, nor is the codebase in any way complete. So we will essentially play catchup for the foreseeable future. The point is not ao necessarily make a commercially viable alternative, it is more to learn about how Xamarin was put together, learn about Avalonia and see what we can make work for fun and for our own legacy projects.  

## Contributing
These are early days and all contributions are welcomed.

## Roadmap

1. Get all of the code from the original Avalonia Forms implementation (which was basically the WPF platform ported to Avalonia with extensions)
2. Implement more complex parts of Xamarin Forms
3. Look at getting some kind of eco-system going.

Legacy Xamarin Forms components will probably nor port easily, as they would eed to first be ported to Avalonia, then to Xamarin Forms Rendered controls, but any control in the Avalonia eco-systems hould be portable to this new platform. 