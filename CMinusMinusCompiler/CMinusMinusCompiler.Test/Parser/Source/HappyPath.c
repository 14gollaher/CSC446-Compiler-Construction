int a, b;
int main()
{
	return -1;
}

const x = 4;

/*This will be omitted!!!*/

float a;
int b, c;

int main()
{
	const horses = 3;
	float d;
	return -1;
}
/*
   Whole bunch of bad stuff
*/

const y = 3;
float func(int z)
{
	return -1;
}

int main()
{
	return -1;
}

int func(int x, int y)
{
	int a, b;
	return -1;
}


/**************************************************/
/* TODO: Figure out why this gives weird errors
int a, b;
int main()
{
	return 4;
}

const x = 4;

float a;
int b, c;

int main()
{
	const horses = 3;
	float d;
	return d;
}

const y = 3;
float func(int z)
{
	return 2;
}

int main()
{
	return y;
}

int func(int x, int y)
{
	int a, b;
	return 2;
}
*/



