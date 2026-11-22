using ApplicationTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationTracker.ViewModels;

public class JobApplicationFormViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Pole Firma jest wymagane")]
    [StringLength(200)]
    public string Company { get; set; } = "";

    [Required(ErrorMessage = "Pole Stanowisko jest wymagane")]
    [StringLength(200)]
    public string Position { get; set; } = "";

    [Required(ErrorMessage = "Pole Lokalizacja jest wymagane")]
    [StringLength(200)]
    public string? Location { get; set; }

    [Required(ErrorMessage = "Pole Tryb pracy jest wymagane")]
    public WorkMode? WorkMode { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryFrom { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalaryTo { get; set; }

    [DataType(DataType.Date)]
    public DateOnly ApplicationDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [StringLength(200)]
    public string? Source { get; set; }

    [Required(ErrorMessage = "Pole Link jest wymagane")]
    [Url(ErrorMessage = "Podaj prawidłowy adres URL")]
    [StringLength(2048)]
    public string? Url { get; set; }

    [StringLength(4000)]
    public string? Notes { get; set; }

    public List<StatusHistoryViewModel> History { get; set; } = [];
}

public class StatusHistoryViewModel
{
    public Guid Id { get; set; }

    public ApplicationStatus Status { get; set; }

    [DataType(DataType.Date)]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [StringLength(2000)]
    public string? Note { get; set; }
}
