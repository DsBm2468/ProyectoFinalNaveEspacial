const int PIN_HORIZONTAL = A0;
const int PIN_VERTICAL = A1;

const int PIN_FIRE = 2;
const int PIN_BOOST = 3;

void setup()
{
    Serial.begin(9600);

    pinMode(
    PIN_FIRE,
    INPUT_PULLUP);

    pinMode(
    PIN_BOOST,
    INPUT_PULLUP);
}

void loop()
{
    int horizontal =
    analogRead(PIN_HORIZONTAL);

    int vertical =
    analogRead(PIN_VERTICAL);

    int fire =
    !digitalRead(PIN_FIRE);

    int boost =
    !digitalRead(PIN_BOOST);

    Serial.print(horizontal);

    Serial.print(",");

    Serial.print(vertical);

    Serial.print(",");

    Serial.print(fire);

    Serial.print(",");

    Serial.println(boost);

    delay(20);
}