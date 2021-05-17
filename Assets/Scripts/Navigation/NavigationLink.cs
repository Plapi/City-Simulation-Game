public class NavigationLink {
	public Construction InputConstruction { get; private set; }
	public Construction OutpoutConstruction { get; private set; }

	public NavigationLink(Construction inputConstruction, Construction outputConstruction) {
		InputConstruction = inputConstruction;
		OutpoutConstruction = outputConstruction;
	}
}