using ispk.models;

namespace ispk.dto {
    public class UserDTO {
	public string? name { get; set; }
	public string? email { get; set; }
	public string? password { get; set; }
	public UserRole role { get; set; } = UserRole.CLIENT;

	public static UserDTO createDtoFromUser(User user) {
	    return new UserDTO {
		name = user.UserName,
		email = user.Email,
		password = user.PasswordHash,
		role = user.role,
	    };
	}
    }
}
