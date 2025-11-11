using ispk.models;

namespace ispk.dto {
    public class MembershipDTO {
	public DateTime? createdAt {get; set;}
	public DateTime? expirationDate {get; set;}

	public UserDTO? user {get; set;}

	public MembershipTypeDTO? membershipType {get; set;}

	public static MembershipDTO createDtoFromMembership(Membership membership) {
	    return new MembershipDTO {
		createdAt = membership.createdAt,
		expirationDate = membership.expirationDate,
		user = UserDTO.createDtoFromUser(membership.user),
		membershipType = MembershipTypeDTO.createDtoFromMembershipType(membership.membershipType)
	    };
	}
    }
}

