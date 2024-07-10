namespace Splitit.Models
{
    public class ActorDTO
    {
        public ActorDTO() { }
        public int Id { get; set; }
        public string Name { get; set; }

        public static ActorDTO ActorToDTO(Actor actor)
        {
            return new ActorDTO { Id = actor.Id, Name = actor.Name };
        }
    }
}
