#TextReport DLL#
----------

## About ##
This library is created so that I can get fast report files from my programs and I am open-sourcing it because I love to share. Its written in a way that with few lines of code you can get tables, text and nice formatting features in the report. I will mainly use it in small projects where I need logging over a time period.

## Documentation ##
For all the classes and methods please check the help file which is under the Documentation folder. Tried to make the help file as clear and simple as possible also I have an example under the examples folder.

## Features ##

> - Can create a text file by choosing the path and name. Or just a file name (it will create the file in the executable folder)
> - Can create tables from 2D string arrays or by using the Table class built-in.
> - Can be used by beginners because you can easily appendText() or breakLine()
> - Saving returns the path of the created file which makes it easy to open
> - Can create a report with only few lines of code.
> - Well documented.

## Changelog ##
**Version 1.1**
> - Changed the line character limiting.
> - Added setEndCharacter() function. (Sets which character will be the last character in line, after the character limit is reached. Makes sure that words are not cut in the middle. End character is set to space by default.)
> - Added fillLineWith() function. (Fills the next line with a specific character.)

**Version 1.0**
> PLEASE SEE THE DOCUMENTATION!

![](https://cdn.meme.am/instances/500x/67126131.jpg)