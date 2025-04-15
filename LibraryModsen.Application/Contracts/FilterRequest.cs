using LibraryModsen.Application.Common;

namespace LibraryModsen.Application.Contracts;

public record class FilterRequest(
    FilterType FilterType = 0,
    string Data = "");
