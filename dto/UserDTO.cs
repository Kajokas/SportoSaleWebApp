using ispk.models;

namespace ispk.dto {
    public class UserDTO {
	public string? name { get; set; }
	public string? email { get; set; }
	public string? password { get; set; }
	public UserRole role { get; set; } = UserRole.CLIENT;
    }
}
