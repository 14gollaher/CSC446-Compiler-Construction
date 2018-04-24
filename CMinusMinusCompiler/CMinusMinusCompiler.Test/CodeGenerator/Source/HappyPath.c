int findArea(int length, int width)
{
	return length * width;
}

int main()
{
	char firstLetterName;
	int length;
	int width;
	int area;

	cout << "Enter the first letter of your name: ";
	cin >> firstLetterName;
	cout << endl << "Hello " << firstLetterName << "!" << endl;

	cout << "Enter length of a rectangle: ";
	cin >> length;
	cout << "Enter width of a rectangle: ";
	cin >> width;

	area = findArea(length, width);

	cout << "The area is: " << area << endl;

	area = (area + 3 * (1 + 1)) - 1; /* area = (6 * area) - 1 */
	cout << "The area after a complex operation is " << area << endl;

	return -1;
}

