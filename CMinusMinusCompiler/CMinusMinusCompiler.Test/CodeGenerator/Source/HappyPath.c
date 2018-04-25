int findArea(int length, int width)
{
	return length * width;
}

int findArea2(int length, int width)
{
	int x;
	x = findArea(length, width);
	return x * 2;
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
	cout << "The area + 1 is: " << area << endl;

	area = findArea2(length, width);
	cout << "The area + (area * 2) is: " << area << endl;

	area = (area + 3 * (1 + 1)) - 1; /* area = area + 5 */
	cout << "The area after a complex operation is: " << area << endl;
	return -1;
}

