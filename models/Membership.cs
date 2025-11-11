using Microsoft.AspNetCore.Identity;
namespace ispk.models;

public class Membership{
    public int id {get; set;}
    public required DateTime createdAt {get; set;}
    public DateTime? expirationDate {get; set;}

    public int userId {get; set;}
    public User user {get; set;}

    public int membershipTypeId {get; set;}
    public MembershipType membershipType {get; set;}
}
