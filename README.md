# MMLib.Tools.Translator

`tr` is .NET Global text translation tool.

## Installation
`tr` is implemented as a .NET Core Global Tool, so its installation is quite simple:

```bash
dotnet tool install -g MMlib.Tools.Translator
```

To update it to the latest version, if it was installed previously, use:

```bash
dotnet tool update -g MMlib.Tools.Translator
```

## Usage

```bash
tr "Hello this is .NET Global text translation tool" -s en -t sk
```

`--source-language (-s)` and `--target-language (-t)` are optional. Default values are from user environment variables `MMLIB_TRANSLATOR_SOURCE_LNG` and `MMLIB_TRANSLATOR_TARGET_LNG`.

## Set default languages

Default languages can be set by `set-defaults` command.

```bash
tr set-defaults -s "en" -t "sk"
```

## Reverse

Mostly a person needs to translate between two languages. If you have the default source and target language set and you want to do the translation just the other way around, you can use the `--reverse (-re)` parameter.

```bash
tr "Ahoj svet" -re
```

> 🙏 @satano thanks for the idea.

## Copy to clipboard

If you need to copy the resulting translated text to the clipboard, you can use the `--copy-to-clipboard (-co)` parameter.

```bash
tr "Hello this is .NET Global text translation tool" --copy-to-clipboard
```
