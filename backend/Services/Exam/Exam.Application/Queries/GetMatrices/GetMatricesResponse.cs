using FrogEdu.Exam.Application.DTOs;

namespace FrogEdu.Exam.Application.Queries.GetMatrices;

public sealed record GetMatricesResponse(IReadOnlyList<MatrixDto> Matrices);
