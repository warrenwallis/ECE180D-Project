'''
Game Object designed to hold all game variables
'''

from enum import Enum
import random as r

class Location(Enum):
    TOP = 1
    MIDDLE = 2
    BOTTOM = 3
class Motion(Enum):
    VERTICAL = 1
    HORIZONTAL = 2

class GameObject():
    def __init__(self):
        self.active = True
        self.player_lives = 3
        self.difficulty = None
        self.box_size = 3

        self.player_actions = {
            'p':'pause',
            'c':'continue',
            'q':'top',
            'a':'middle',
            'z':'bottom',
            'j':'vertical',
            'k':'horizontal'
        }

        self.blocks = {
            Motion.VERTICAL.value:'* = *',
            Motion.HORIZONTAL.value:'*===*',
            3:'*****',
            4:'*   *'
        }

    def set_difficulty(self):
        while self.difficulty is None:
            difficulty = input(f'Choose Difficulty from 3 (Easy) to 1 (Hard): ')
            if difficulty.isdigit() and int(difficulty) <= 3:
                self.difficulty = int(difficulty)
            else:
                print(f'Choice {difficulty} is invalid')

    def play(self):
        self.print_field(r.randint(1,len(Location)),r.randint(1,len(Motion)))

    def print_field(self, location, motion):
        for i in range(1,4):
            print(self.create_box(motion if location == i else None))

    def create_box(self, motion=None):
        output = self.blocks[3] + '\n'

        match motion:
            case None:
                for i in range(self.box_size):
                    output += self.blocks[4] + '\n'
            case Motion.VERTICAL.value:
                for i in range(self.box_size):
                    output += self.blocks[motion] + '\n'
            case default:
                output += self.blocks[4] + '\n'
                output += self.blocks[motion] + '\n'
                output += self.blocks[4] + '\n'

        output += self.blocks[3]

        return output
