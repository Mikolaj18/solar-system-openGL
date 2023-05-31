using GLFW; //Przestrzeń nazw biblioteki GLFW.NET. Zawiera ona bindingi do biblioteki GLFW zapewniającej możliwość tworzenia aplikacji wykorzystujacych OpenGL
using GlmSharp; //Przestrzeń nazw biblioteki GlmSharp. GlmSharp to port biblioteki GLM - OpenGL Mathematics implementującej podstawowe operacje matematyczne wykorzystywane w grafice 3D.

using Shaders; //Przestrzeń nazw pomocniczej biblioteki do wczytywania programów cieniującch
using Models; //Przestrzeń nazw pomocniczej biblioteki rysującej kilka przykładowych modeli

using System;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using System.Drawing;
using System.Numerics;

namespace PMLabs
{


    //Implementacja interfejsu dostosowującego metodę biblioteki Glfw służącą do pozyskiwania adresów funkcji i procedur OpenGL do współpracy z OpenTK.
    public class BC : IBindingsContext
    {
        public IntPtr GetProcAddress(string procName)
        {
            return Glfw.GetProcAddress(procName);
        }
    }

    class Program
    {

        //Metoda wykonywana po zainicjowaniu bibliotek, przed rozpoczęciem pętli głównej
        //Tutaj umieszczamy nasz kod inicjujący
        public static void InitOpenGLProgram(Window window)
        {
            GL.ClearColor(0, 0, 0, 1); //Wyczyść zawartość okna na czarno (r=0,g=0,b=0,a=1)
            DemoShaders.InitShaders("Shaders\\"); //Wczytaj przykładowe programy cieniujące
        }

        //Metoda wykonywana po zakończeniu pętli główej, przed zwolnieniem zasobów bibliotek
        //Tutaj zwalniamy wszystkie zasoby zaalokowane na począdku programu
        public static void FreeOpenGLProgram(Window window)
        {

        }
        static Sphere sun = new Sphere(5f, 72f, 72f);

        static Sphere mercury = new Sphere(0.19f, 36f, 36f); //Merkury

        static Sphere venus = new Sphere(0.47f, 36f, 36f); //Merkury

        static Sphere earth = new Sphere(0.5f, 36f, 36f); //Ziemia o promieniu 0.5f
        static Sphere earth_moon = new Sphere(0.13f, 36f, 36f); //Księżyc Ziemii, promień przeskalowany względem Ziemii

        static Sphere mars = new Sphere(0.26f, 36f, 36f); //Mars, promień przeskalowany względem Ziemii
        static Sphere phobos = new Sphere(0.00086f, 36f, 36f); //Fobos, jeden z księżyców Marsa (jest dość mikroskopijny), promień przeskalowany względem Ziemii
        static Sphere deimos = new Sphere(0.00047f, 36f, 36f); //Deimos, jeden z księżyców Marsa (jeszcze mniejszy niż fobos, ciężki do dostrzeżenia), promień przeskalowany względem Ziemii

        static Sphere jupiter = new Sphere(1.30f, 36f, 36f); //Jowisz

        static Sphere saturn = new Sphere(0.95f, 36f, 36f); //Saturn

        static Sphere uranium = new Sphere(0.65f, 36f, 36f); //Uran

        static Sphere neptunium = new Sphere(0.60f, 36f, 36f); //Neptun

        static Sphere pluto = new Sphere(0.093f, 36f, 36f); //Pluton, promień przeskalowany względem Ziemii
        static Sphere charon = new Sphere(0.04755f, 36f, 36f); //Charon - księżyc plutona, promień przeskalowany względem Ziemii


        //Metoda wykonywana najczęściej jak się da. Umieszczamy tutaj kod rysujący
        public static void DrawScene(Window window, float angle)
        {
            // Wyczyść zawartość okna (buforów kolorów i głębokości)
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Tu rysujemy
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            mat4 V = mat4.LookAt(
                new vec3(0.0f, 0.0f, -50.0f),
                new vec3(0.0f, 0.0f, 0.0f),
                new vec3(0.0f, 1.0f, 0.0f));
            mat4 P = mat4.Perspective(glm.Radians(70.0f), 1.0f, 1.0f, 50.0f);

            DemoShaders.spConstant.Use();//Aktywacja programu cieniującego
            GL.UniformMatrix4(DemoShaders.spConstant.U("P"), 1, false, P.Values1D);
            GL.UniformMatrix4(DemoShaders.spConstant.U("V"), 1, false, V.Values1D);


            mat4 Mt1 = mat4.Identity;
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt1.Values1D);
            sun.drawWire();

            //Merkury
            mat4 Mt2 = Mt1;
            Mt2 = Mt2 * mat4.Rotate(angle / 2.41f, new vec3(0f, 0f, -1.0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 88 dni wynosi około angle / 2.41f 
            Mt2 = Mt2 * mat4.Translate(new vec3(-8f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt2_1 = Mt2 * mat4.Rotate(angle / 1.58f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, to 87 dni to około angle / 1.58f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt2_1.Values1D);
            mercury.drawWire();

            //Wenus
            mat4 Mt9 = Mt1;
            Mt9 = Mt9 * mat4.Rotate(angle / 6.13f, new vec3(0f, 0f, -1.0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 224 dni wynosi około angle / 6.13f 
            Mt9 = Mt9 * mat4.Translate(new vec3(-10f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt9_1 = Mt9 * mat4.Rotate(angle / 6.65f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, to 243 dni to około angle / 6.65f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt9_1.Values1D);
            venus.drawWire();

            //Ziemia
            mat4 Mt6 = Mt1;
            Mt6 = Mt6 * mat4.Rotate(angle / 10, new vec3(0f, 0f, -1.0f)); //obrót w okół słońca - 365 dni - 10 sekund
            Mt6 = Mt6 * mat4.Translate(new vec3(-12f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt6_1 = Mt6 * mat4.Rotate(angle / 0.027f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, to 1 dzień to angle / 0.027f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt6_1.Values1D);
            earth.drawWire();

            //Księżyc Ziemi
            mat4 Mt7 = Mt6;
            Mt7 = Mt7 * mat4.Rotate(angle / 0.73f, new vec3(1.0f, 0f, 0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 27 dni wynosi angle / 0.73f 
            Mt7 = Mt7 * mat4.Translate(new vec3(0f, 1f, 0f));
            Mt7 = Mt7 * mat4.Rotate(angle / 0.73f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi trwa dokładnie tyle samo ile obrót w okół Ziemi
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt7.Values1D);
            earth_moon.drawWire();


            //Mars
            mat4 Mt3 = Mt1;
            Mt3 = Mt3 * mat4.Rotate(angle / 18.8f, new vec3(0f, 0f, -1.0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 687 dni wynosi angle / 0.2.41f 
            Mt3 = Mt3 * mat4.Translate(new vec3(-14f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt3_1 = Mt3 * mat4.Rotate(angle / 0.02756f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, praktycznie identyczny jak Ziemia, niewiele większy
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt3_1.Values1D);
            mars.drawWire();

            //Fobos - księżyc Marsa
            mat4 Mt4 = Mt3;
            Mt4 = Mt4 * mat4.Rotate(angle / 0.0078f, new vec3(1.0f, 0f, 0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 7 godzin wynosi angle / 0.0078f 
            Mt4 = Mt4 * mat4.Translate(new vec3(0f, 1f, 0f)); //posiada orbitę bliżej Marsa
            Mt4 = Mt4 * mat4.Rotate(angle / 0.0078f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, synchroniczny do ruchu obiegowego
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt4.Values1D);
            phobos.drawWire();

            //Deimos - księżyc Marsa
            mat4 Mt5 = Mt3;
            Mt5 = Mt5 * mat4.Rotate(angle / 0.0337f, new vec3(1.0f, 0f, 0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 30 godzin wynosi angle / 0.0078f 
            Mt5 = Mt5 * mat4.Translate(new vec3(0f, 2f, 0f)); //posiada orbite bardziej oddaloną od Marsa
            Mt5 = Mt5 * mat4.Rotate(angle / 0.0337f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, synchroniczny do ruchu obiegowego
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt5.Values1D);
            deimos.drawWire();


            //Jowisz
            mat4 Mt8 = Mt1;
            Mt8 = Mt8 * mat4.Rotate(angle / 120, new vec3(0f, 0f, -1.0f)); //obrót w okół słońca - 365 dni - 10 sekund, czyli 12 (po zaokrągleniu) lat wynosi około angle / 120 
            Mt8 = Mt8 * mat4.Translate(new vec3(-18f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt8_1 = Mt8 * mat4.Rotate(angle / 0.01125f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, to 10 godzin dzień to angle / 0.01125f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt8_1.Values1D);
            jupiter.drawWire();

            //Saturn
            mat4 Mt10 = Mt1;
            Mt10 = Mt10 * mat4.Rotate(angle / 290, new vec3(0f, 0f, -1.0f)); //obrót w okół słońca - 365 dni - 10 sekund, czyli 29 (po zaokrągleniu) lat wynosi około angle / 290 
            Mt10 = Mt10 * mat4.Translate(new vec3(-22.5f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt10_1 = Mt10 * mat4.Rotate(angle / 0.01181f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, nieznacznie więcej niż Jowisz
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt10_1.Values1D);
            saturn.drawWire();

            //Uran
            mat4 Mt11 = Mt1;
            Mt11 = Mt11 * mat4.Rotate(angle / 840, new vec3(0f, 0f, -1.0f)); //obrót w okół słońca - 365 dni - 10 sekund, czyli 84 (po zaokrągleniu) lat wynosi około angle / 840 
            Mt11 = Mt11 * mat4.Translate(new vec3(-26f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt11_1 = Mt11 * mat4.Rotate(angle / 0.019125f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, 17 godzin wynosi około angle / 0.019125f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt11_1.Values1D);
            uranium.drawWire();

            //Neptun
            mat4 Mt12 = Mt1;
            Mt12 = Mt12 * mat4.Rotate(angle / 1650, new vec3(0f, 0f, -1.0f)); //obrót w okół słońca - 365 dni - 10 sekund, czyli 165 (po zaokrągleniu) lat wynosi około angle / 1650 
            Mt12 = Mt12 * mat4.Translate(new vec3(-28f, 0.0f, 0.0f)); //oddalenie od słonća
            mat4 Mt12_1 = Mt12 * mat4.Rotate(angle / 0.018f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, 16 godzin wynosi około angle / 0.018f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt12_1.Values1D);
            neptunium.drawWire();

            //Pluton
            mat4 Mt13 = Mt1;
            Mt13 = Mt13 * mat4.Rotate(angle / 2480, new vec3(0f, 0f, -1.0f)); //obrót w okół słońca - 365 dni - 10 sekund, czyli 248 (po zaokrągleniu) lat wynosi około angle / 1650 
            Mt13 = Mt13 * mat4.Translate(new vec3(-30f, -10.0f, 0.0f)); //oddalenie od słonća + zmiana położenia płaszczyzny orbbity
            mat4 Mt13_1 = Mt13 * mat4.Rotate(angle / 0.172f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi, 10 sekund = angle / 10, 153 godziny wynosi około angle / 0.172f
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt13_1.Values1D);
            pluto.drawWire();

            //Charon - księżyc plutona
            mat4 Mt14 = Mt13;
            Mt14 = Mt14 * mat4.Rotate(angle / 0.164f, new vec3(1.0f, 0f, 0f)); // obrót w okół ziemi - angle / 10 = 365 dni, to 6 dni wynosi angle / 0.164f 
            Mt14 = Mt14 * mat4.Translate(new vec3(0f, 1f, 0f));
            Mt14 = Mt14 * mat4.Rotate(angle / 0.164f, new vec3(0.0f, 1.0f, 0.0f)); //obrót w okół własnej osi trwa dokładnie tyle samo ile obrót w okół Plutona
            GL.UniformMatrix4(DemoShaders.spConstant.U("M"), 1, false, Mt14.Values1D);
            charon.drawWire();





            //Skopiuj ukryty bufor do bufora widocznego            
            Glfw.SwapBuffers(window);
        }

        static float speed = 3.14f * 2; // [radiany/s] - obrót o 360 stopni to 2π radianów (ok. 6,28 radiana)

        //Metoda główna
        static void Main(string[] args)
        {
            Glfw.Init();//Zainicjuj bibliotekę GLFW

            Window window = Glfw.CreateWindow(1500, 1500, "OpenGL", GLFW.Monitor.None, Window.None); //Utwórz okno o wymiarach 500x500 i tytule "OpenGL"

            Glfw.MakeContextCurrent(window); //Ustaw okno jako aktualny kontekst OpenGL - tutaj będą realizowane polecenia OpenGL
            Glfw.SwapInterval(1); //Skopiowanie tylnego bufora na przedni ma się rozpocząć po zakończeniu aktualnego odświerzania ekranu

            GL.LoadBindings(new BC()); //Pozyskaj adresy implementacji poszczególnych procedur OpenGL

            InitOpenGLProgram(window); //Wykonaj metodę inicjującą Twoje zasoby 

            float angle = 0;
            Glfw.Time = 0;

            while (!Glfw.WindowShouldClose(window)) //Wykonuj tak długo, dopóki użytkownik nie zamknie okna
            {
                angle += speed * (float)Glfw.Time;
                Glfw.Time = 0;
                DrawScene(window, angle); //Wykonaj metodę odświeżającą zawartość okna
                Glfw.PollEvents(); //Obsłuż zdarzenia użytkownika
            }


            FreeOpenGLProgram(window);//Zwolnij zaalokowane przez siebie zasoby

            Glfw.Terminate(); //Zwolnij zasoby biblioteki GLFW
        }


    }
}