void setup() {
  // put your setup code here, to run once:
pinMode(2, OUTPUT); 
pinMode(3, OUTPUT);
pinMode(4, OUTPUT); 
pinMode(5, OUTPUT);
pinMode(6, OUTPUT); 
pinMode(7, OUTPUT);
pinMode(8, OUTPUT);
pinMode(9, OUTPUT);
pinMode(10, OUTPUT);

  Serial.begin(9600);    
    
}    
    
void loop() {    
  // put your main code here, to run repeatedly:    
 
if(Serial.available()){ 
     int a=Serial.read();
     if(a=='1') {   
  digitalWrite(2,HIGH);}    
  else if(a=='2'){
  digitalWrite(3,HIGH);
  digitalWrite(2,LOW);}
  else if(a=='3'){
  digitalWrite(4,HIGH);
  digitalWrite(3,LOW);}
  else if(a=='4'){
  digitalWrite(5,HIGH);
  digitalWrite(4,LOW);}  
  else if(a=='5'){
  digitalWrite(6,HIGH);
  digitalWrite(5,LOW);}
  else if(a=='6'){
  digitalWrite(7,HIGH);
  digitalWrite(6,LOW);}
  else if(a=='8'){
  digitalWrite(8,HIGH);
  digitalWrite(7,LOW);}
  else if(a=='9'){
  digitalWrite(9,HIGH);
  digitalWrite(8,LOW);}
  
}

else digitalWrite(9,LOW);

}

