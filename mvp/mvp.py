import time as t
from game_object import GameObject

def main():
    game = GameObject()
    game.set_difficulty()
    game.play()
    print('Exiting Game')

if __name__=='__main__':
    # define game variables
    main()