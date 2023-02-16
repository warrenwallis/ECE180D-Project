'''
Game Object designed to hold all game variables
'''

from enum import Enum
import random as r
import multiprocessing as mp
from threading import Thread
import sys

global user_input
active = True

class Location(Enum):
    TOP = 1
    MIDDLE = 2
    BOTTOM = 3
class Motion(Enum):
    VERTICAL = 1
    HORIZONTAL = 2

class GameObject():
    def __init__(self):
        self.player_lives = 3
        self.difficulty = None
        self.box_size = 3
        self.player_score = 0
        self.timer = None

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
        while self.player_lives > 0:
            global user_input
            global active
            user_input = None
            
            if active is True:
                active = False
                print(f'Player Lives: {self.player_lives}, Player Score: {self.player_score}')
                location,motion = r.randint(1,len(Location)),r.randint(1,len(Motion))
                self.print_field(location,motion)

                p = Thread(target=get_user_input, args=[])
                p.daemon = True
                p.start()
                p.join(self.difficulty)
                
                print(f'\nReceived user input: {user_input}')
                if user_input is None:
                    print(f'\nWrong Action {user_input}! Decrement Life :(')
                    print(f'press any key to continue')
                    self.player_lives -= 1



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

def get_user_input():
    global user_input, active
    user_input = input(f'Type action: q = top, a = middle, z = bottom; j = vertical, k = horizontal: ')
    active = True