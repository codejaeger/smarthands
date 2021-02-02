const int buzzer = 9;
const int buzzer1 = 10; //buzzer to arduino pin 9
int mode = 1;

void setup(){
 
  pinMode(buzzer, OUTPUT); // Set buzzer - pin 9 as an output
  pinMode(buzzer1, OUTPUT);
}

void loop(){
  if(mode==1)
  {
  tone(buzzer, 1000); // Send 1KHz sound signal...
  delay(50);        // ...for 1 sec
  noTone(buzzer);     // Stop sound...
  delay(1000);        // ...for 1sec 
  }
  else
  {
  tone(buzzer1, 1000); // Send 1KHz sound signal...
  delay(3000);        // ...for 1 sec
  noTone(buzzer1);     // Stop sound...
  delay(1000);        // ...for 1sec 
  }
  mode=(mode+1)%2;
}
