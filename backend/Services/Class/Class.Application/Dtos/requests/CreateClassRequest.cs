namespace FrogEdu.Class.Application.Dtos.requests;

public sealed record CreateClassRequest(
    string Name,
    string Description,
    string Grade,
    int MaxStudents,
    string? BannerUrl
);
