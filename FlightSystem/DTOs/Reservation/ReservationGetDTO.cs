using FlightSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlightSystem.DTOs.Reservation
{
    public class ReservationGetDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string UserFullName { get; set; }

        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public string SeatNumber { get; set; }
        public int? AdditionalBaggageId { get; set; }
        public string? AdditionalBaggageType { get; set; }

        public decimal TotalPrice { get; set; }
        public StatusReservation Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
