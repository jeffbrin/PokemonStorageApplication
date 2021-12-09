import pandas as pd

moves_df = pd.read_csv('PokemonMoves.csv')

moves_df = moves_df[moves_df['Power'] != '-']

moves_df.to_csv('PokemonMoves.csv', index=False)
