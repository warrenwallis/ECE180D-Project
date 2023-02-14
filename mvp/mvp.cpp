#include <iostream>
#include <string>
#include <unordered_map>

void print(std::string);

int main() {
    // define game variables
    std::unordered_map<char, std::string> playerActions = {
        {'c', "continue"},
        {'p', "pause"},
        {'q', "topLeftSwipe"},
        {'w', "topRightSwipe"},
        {'a', "bottomLeftSwipe"},
        {'s', "bottomRightSwipe"}
    };

    for (const auto [key, val] : playerActions)
        print(val);

    return 0;
}

void print(std::string output) {
    std::cout << output << std::endl;
}