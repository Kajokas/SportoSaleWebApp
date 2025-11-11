using Microsoft.AspNetCore.Identity;
namespace ispk.models;

public class MembershipType {
    public int id {get; set;}
    public required string name {get; set;}
    public required decimal price {get; set;}
    public required TimeSpan validity {get; set;}

    public ICollection<Membership> memberships { get; set; }
}

