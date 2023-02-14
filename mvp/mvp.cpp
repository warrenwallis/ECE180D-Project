#include <iostream>
#include <string>
#include <unordered_map>

// define some constants
std::unordered_map<std::string, std::string> playerActions ({
    {"pause", "pause"}, 
    {"play", "play"}
});

void print(std::string);

int main() {
    print("Begin ECE180D MVP");

    for (auto [key, val] : playerActions)
        print(val);
}

void print(std::string output) {
    std::cout << output << std::endl;
}