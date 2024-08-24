// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "type", Target = "~T:CuoreUI.Controls.cuiSlider")]

//disabled because they're annoying

[assembly: SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>", Scope = "module")]

//cant do new() nor use primary constructor because we build for not only 8.0

[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>", Scope = "module")]
[assembly: SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>", Scope = "module")]
