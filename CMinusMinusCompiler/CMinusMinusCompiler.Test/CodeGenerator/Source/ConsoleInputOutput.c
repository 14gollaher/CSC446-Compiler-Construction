int alpha;
char beta;

char baz(int foo, char zeta)
{
	int gamma;
	cout << "Hello!" << endl << endl;
	cout << "Enter a char for zeta: ";
	cin >> zeta;
	cout << "Enter an int for gamma: ";
	cin >> gamma;
	cout << "Enter an int for alpha: ";
	cin >> alpha;

	cout << "The value of alpha is: " << alpha << endl
		<< "The value of zeta is: " << zeta << endl
		<< "The value of gamma is: " << gamma << endl;
	return zeta;
}

int main()
{
	int delta;
	char charlie;
	charlie = baz(delta, charlie);
	cout << "The value of charlie is: " << charlie << endl;
	return delta;
}

