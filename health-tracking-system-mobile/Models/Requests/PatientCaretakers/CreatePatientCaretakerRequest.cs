﻿using System.ComponentModel.DataAnnotations;

namespace health_tracking_system_mobile.Models.Requests.PatientCaretakers;

public class CreatePatientCaretakerRequest
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [Required] public string Patronymic { get; set; } = null!;
    [Required] public string Phone { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    [Required] public DateTime BirthDate { get; set; }
    [Required] public string Password { get; set; } = null!; // _QGrXyvcmTD4aVQJ_ for tests
    public List<string>? PatientsIds { get; set; }
}