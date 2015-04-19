# Outline #

  1. gather data
  1. start with alpha channel PNGs from ponibooru et al
    1. decide how to annotate images fast/best
      1. build a web application for crowdsourcing?
      1. what information do we need?
  1. come up with good features
    1. color - ponies have distinctive colors. sometimes this does not hold true.
    1. cutie mark - a 2d image - easy, all invariances are known
    1. shape? - not very good on its own as many ponies share the same model, but crucial for bw images and recolors. also, how do we deal with humanized mlp?
    1. remember: we don’t have to do this if we have lots of data and computing power. but we don’t.
    1. color histogram
    1. neural net + sliding windows?
      1. what’s the smallest possible pony?
  1. implement the system
    1. choose platform vs cross platform
    1. modularity
    1. use cases and applications
      1. automatic organization of the pony folder
      1. pony tineye
      1. automatic transcription of episodes
    1. what is the pipeline?
      1. hisrogram -> sliding window -> neural net