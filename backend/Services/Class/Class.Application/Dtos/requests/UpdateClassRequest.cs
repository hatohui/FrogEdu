namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record UpdateClassRequest(
    string Name,
    string Grade,
    int MaxStudents,
    string? BannerUrl
);
