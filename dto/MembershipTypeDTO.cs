using ispk.models;

namespace ispk.dto {
    public class MembershipTypeDTO {
	public string? name {get; set;}
	public decimal? price {get; set;}

	public static MembershipTypeDTO createDtoFromMembershipType(MembershipType membershipType) {
	    return new MembershipTypeDTO {
		name = membershipType.name,
		price = membershipType.price
	    };
	}
    }
}


