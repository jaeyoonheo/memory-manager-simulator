# Memory Replacement Policy Simulator

### Description

Operating system lecture term project. Implemented the Memory Replacement Policy performance evaluation tool as a GUI.



### Environment

This code is written on Windows 10, using the Visual Studio 2017 compiler.



### Scheduling Algorithm

#### Creteria

+ Hit

  **If the requested page already exists in main memory**
+ Page Fault

  **The requested page does not exist in main memory. As the number of page faults increases, the overhead increases.**
+ Page Fault Rate

  **(Page Falut) / (Reference String Length)**

#### 1. FIFO (First In First Out)

>  FIFO selects the page that arrives first as Victim. Because it page-outs the reference string that arrives first, it is performed without the overhead for selecting the victim separately.

#### 2. Optimal

>  The Optimal algorithm selects the frame with the lowest probability of being used in the future as Victim. The fact that it will be used in the future is learned through the entire Reference String, and the page that is the latest referenced or is not scheduled to be referenced in the future is the target of replacement.

#### 3. LRU - Counter Implementation

>  Counter Implementation is one of the LRU implementation methods, and it is a method to record the counter. LRU is an abbreviation of Least Recently Used and is an algorithm that selects the oldest page as Victim. The Counter added to implement this is when a page is loaded into memory and referenced, it updates the counter to store information about when it was last referenced.

#### 4. LRU - Stack Implementation

>  Stack Implementation is one of the LRU implementation methods, and records the order in which pages were requested through the stack data structure. The page requested the latest is at the top of the stack, and the page requested the earliest is at the bottom of the stack. Therefore, it is a method of replacing the lowest page.

#### 5. Additional Reference Bit Algorithm

>  This algorithm is an approximate policy to reduce the implementation complexity of LRU. Each frame adds a Reference Bit with 8 bits. When the page is re-referenced, the leftmost bit of Bit is set to 1. By shifting the Reference Bit to the right by 1 for every page request, it is possible to determine whether the current bit has been re-referenced or not.

#### 6. Second-Chance Algorithm

>  The corresponding algorithm is also an approximate policy to reduce the implementation complexity of LRU. Each frame uses a reference bit of 1-bit size. The Reference Bit is cycled repeatedly in a cyclic structure, and the Reference Bit is initialized to 0.

#### 7. NUR (Not Used Recently) Algorithm

>  It is an improved algorithm from Second-Chance Algorithm. It is checked by adding the Dirty Bit that checks whether the current frame has recently changed to the Reference Bit. The priority for this is as follows. (The higher the table, the higher the replacement target rank)

#### 8. LFU (Least Frequency Used) & MFU (Most Frequency Used) Algorithm

>  LFU and MFU are policies in which the replacement target is determined by Frequency and frequency of use. LFU becomes a replacement target when the number of pages referenced so far is the least, and MFU becomes a replacement target when the number of references is the largest.



### Usage

Random : Create random reference string 

Run : Simulation display

Frame : space for memory
