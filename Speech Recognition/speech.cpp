#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <sstream>
using namespace std;
 


void print_screen(int screen, string song){
    if(screen == 1){
        cout << "(Main Screen)" << endl;
        cout << "1. Play" << endl;
        cout << "2. Tutorial" << endl;
        cout << "3. Pick Song" << endl;
        cout << "4. Settings" << endl;
        cout << "5. Exit" << endl;
    }
    else if(screen == 2){
        cout << "(Play Screen)" << endl;
        cout << "1. Back" << endl;
        cout << "2. Exit" << endl;
    }
    else if(screen == 3){
        cout << "(Tutorial Screen)" << endl;
        cout << "1. Back" << endl;
        cout << "2. Exit" << endl;
    }
    else if(screen == 4){
        cout << "(Pick Song Screen)" << endl;
        cout << "Your Selection :" << song << endl;
        cout << "1. The Crab Song" << endl;
        cout << "2. The Other Song" << endl;
        cout << "3. Back" << endl;
        cout << "4. Exit" << endl;
    }
    else if(screen == 5){
        cout << "(Settings Screen)" << endl;
        cout << "1. Back" << endl;
        cout << "2. Exit" << endl;
    }
}

int new_screen(int screen, vector<string> outputs, string & song){
    if (find(outputs.begin(), outputs.end(), "exit") != outputs.end()) {
        exit(0);
    }
    else if (find(outputs.begin(), outputs.end(), "back") != outputs.end()) {
        print_screen(1, song);
        return 1;
    }
    else if(screen == 1){
        if (find(outputs.begin(), outputs.end(), "play") != outputs.end()) {
            print_screen(2, song);
            return 2;
        }
        else if(find(outputs.begin(), outputs.end(), "tutorial") != outputs.end()) {
            print_screen(3, song);
            return 3;
        }
        else if(find(outputs.begin(), outputs.end(), "pick song") != outputs.end()) {
            print_screen(4, song);
            return 4;
        }
        else if(find(outputs.begin(), outputs.end(), "settings") != outputs.end()) {
            print_screen(5, song);
            return 5;
        }
    }
    else if(screen == 4){
        if (find(outputs.begin(), outputs.end(), "the crab song") != outputs.end()) {
            song = "The Crab Song";
            print_screen(4, "The Crab Song");
        }
        else if (find(outputs.begin(), outputs.end(), "the other song") != outputs.end()) {
            song = "The Other Song";
            print_screen(4, "The Other Song");
        }
        return 4;
    }
    return screen;
}

int main()
{
    string filename;
    filename = "outputs.txt";
    int screen_number = 1;
    string song = "";
    print_screen(screen_number, "");

    string line;   // To read each line from code
    int count=0;    // Variable to keep count of each line
    int temp_count = -1;
    while(true){
        ifstream mFile (filename);  
        if(mFile.is_open())
        {
            while(mFile.peek()!=EOF)
            {
                getline(mFile, line);
                // cout << line << endl;
                count++;
            }
            mFile.close();
            if(count != temp_count){
                // cout<<"Number of lines in the file are: "<<count<<endl;
                temp_count = count;
                // cout << line << endl;
                vector<string> v;
 
                stringstream ss(line);
            
                while (ss.good()) {
                    string substr;
                    getline(ss, substr, ',');
                    v.push_back(substr);
                }
                screen_number = new_screen(screen_number, v, song);
            }
            count = 0;

        }
        else{
            cout<<"Couldn't open the file\n";
        }
    }
    
    
    return 0;
}