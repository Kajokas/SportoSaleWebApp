using Microsoft.AspNetCore.Identity;
namespace ispk.models;

public enum UserRole {
    ADMIN,
    CLIENT,
    TRAINER
}

public class User: IdentityUser<int> {
    public required UserRole role { get; set; } = UserRole.CLIENT;
}
