using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
class Solve{
    public Solve(){}
    StringBuilder sb;
    ReadData re;
    public static int Main(){
        new Solve().Run();
        return 0;
    }
    void Run(){
        sb = new StringBuilder();
        re = new ReadData();
        Calc();
        Console.Write(sb.ToString());
    }
    void Calc(){
        uint a = (1 << 5) + (1 << 10);
        Console.WriteLine(a >> 7);
        Console.WriteLine((a << 26) >> 26);
        
        Console.WriteLine(a + " "+ (a+a));

        Console.WriteLine(~a);
        SHA256 sha256 = new SHA256();
        sha256.WriteHash("");
        sha256.WriteHash(""+(char)(0));
        sha256.WriteHash(""+(char)(1));
        sha256.WriteHash(".");
        sha256.WriteHash("The quick brown fox jumps over the lazy dog");
        sha256.WriteHash("The quick brown fox jumps over the lazy dog.");
        sha256.WriteHash(re.s());
        //sb.Append(count+"\n");
    }
}
class SHA256{
    uint[] init_hash_values;
    uint[] k_values;
    public SHA256(){
        init_hash_values = new uint[8]{0x6a09e667, 0xbb67ae85,
                    0x3c6ef372, 0xa54ff53a,
                    0x510e527f, 0x9b05688c,
                    0x1f83d9ab, 0x5be0cd19};
        k_values = new uint[64]{0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b, 0x59f111f1, 0x923f82a4, 0xab1c5ed5,
            0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3, 0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174,
            0xe49b69c1, 0xefbe4786, 0x0fc19dc6, 0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da,
            0x983e5152, 0xa831c66d, 0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967,
            0x27b70a85, 0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
            0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585, 0x106aa070,
            0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a, 0x5b9cca4f, 0x682e6ff3,
            0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa, 0xa4506ceb, 0xbef9a3f7, 0xc67178f2};
    }
    public void WriteHash(string S){
        uint[] hash = Hash(S);
        for(int i=0;i<8;i++){
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
            ToO(hash[i] / (1 << 28));
            hash[i] = hash[i] << 4;
        }
        Console.WriteLine();
    }
    void ToO(uint c){
        if(c < 10){
            Console.Write((char)(c+'0'));
        }
        else{
            Console.Write((char)(c-10+'a'));
        }
    }
    public uint[] Hash(string S){
        return BitToHash(ToBinary(S));
    }
    uint[] ToBinary(string S){
        int bit = S.Length * 8 + 1;
        bit += 64;
        if(bit % 512 != 0){
            bit += 512 - (bit % 512);
        }
        Binary ans = new Binary(bit);
        for(int i=0;i<S.Length;i++){
            ans.WriteChar(S[i]);
        }
        ans.WriteBit(1);
        for(int i=S.Length*8+1;i%512 != 448;i++){
            ans.WriteBit(0);
        }
        ans.WriteLong((ulong)(S.Length * 8));
        return ans.A; 
    }
    uint[] BitToHash(uint[] U){
        uint[] ans = new uint[8];
        for(int i=0;i<8;i++){
            ans[i] = init_hash_values[i];
        }
        for(int block=0;block<U.Length/16;block++){
            uint[] w = new uint[64];
            for(int i=0;i<16;i++){
                w[i] = U[block*16+i];
            }
            for(int i=16;i<64;i++){
                uint s0 = (w[i-15] >> 7) ^ (w[i-15] << 25) ^ (w[i-15] >> 18) ^ (w[i-15] << 14) ^ (w[i-15] >> 3);
                uint s1 = (w[i-2] >> 17) ^ (w[i-2] << 15) ^ (w[i-2] >> 19) ^ (w[i-2] << 13) ^ (w[i-2] >> 10);
                w[i] = w[i-16] + w[i-7] + s0 + s1;
            }
            uint a = ans[0];
            uint b = ans[1];
            uint c = ans[2];
            uint d = ans[3];
            uint e = ans[4];
            uint f = ans[5];
            uint g = ans[6];
            uint h = ans[7];
            for(int i=0;i<63;i++){
                uint S1 = (e >> 6) ^ (e << 26) ^ (e >> 11) ^ (e << 21) ^ (e >> 25) ^ (e << 7);
                uint ch = (e & f) ^ ((~e) & g);
                uint temp1 = h + S1 + ch + w[i] + k_values[i];
                uint S0 = (a >> 2) ^ (a << 30) ^ (a >> 13) ^ (a << 19) ^ (a >> 22) ^ (a << 10);
                uint maj = (a & b) ^ (a & c) ^ (b & c);
                uint temp2 = S0 + maj;
                h = g;
                g = f;
                f = e;
                e = d + temp1;
                d = c;
                c = b;
                b = a;
                a = temp1 + temp2;
            }
            ans[0] += a;
            ans[1] += b;
            ans[2] += c;
            ans[3] += d;
            ans[4] += e;
            ans[5] += f;
            ans[6] += g;
            ans[7] += h;
        }
        return ans;
    }
}
//32bit単位のみ
struct Binary{
    public uint[] A;
    int p;
    public Binary(int N){
        A = new uint[N / 32];
        p = 0;
    }
    //きりのいい時だけ
    public void WriteChar(char c){
        A[p/32] = (A[p/32] << 8) + c; 
        p += 8;
    }
    public void WriteBit(uint c){
        A[p/32] = (A[p/32] << 1) + c; 
        p++;
    }
    //きりのいい時だけ
    public void WriteLong(ulong L){
        A[p/32] = (uint)(L / (((ulong)(1)) << 32));
        p += 32;
        A[p/32] = (uint)(L % (((ulong)(1)) << 32));
        p += 32;
    }
}

class ReadData{
    string[] str;
    int counter;
    public ReadData(){
        counter = 0;
    }
    public string s(){
        if(counter == 0){
            str = Console.ReadLine().Split(' ');
            counter = str.Length;
        }
        counter--;
        return str[str.Length-counter-1];
    }
    public int i(){
        return int.Parse(s());
    }
    public long l(){
        return long.Parse(s());
    }
    public double d(){
        return double.Parse(s());
    }
    public int[] ia(int N){
        int[] ans = new int[N];
        for(int j=0;j<N;j++){
            ans[j] = i();
        }
        return ans;
    }
    public int[] ia(){
        str = Console.ReadLine().Split(' ');
        counter = 0;
        int[] ans = new int[str.Length];
        for(int j=0;j<str.Length;j++){
            ans[j] = int.Parse(str[j]);
        }
        return ans;
    }
    public long[] la(int N){
        long[] ans = new long[N];
        for(int j=0;j<N;j++){
            ans[j] = l();
        }
        return ans;
    }
    public long[] la(){
        str = Console.ReadLine().Split(' ');
        counter = 0;
        long[] ans = new long[str.Length];
        for(int j=0;j<str.Length;j++){
            ans[j] = long.Parse(str[j]);
        }
        return ans;
    }
    public double[] da(int N){
        double[] ans = new double[N];
        for(int j=0;j<N;j++){
            ans[j] = d();
        }
        return ans;
    }
    public double[] da(){
        str = Console.ReadLine().Split(' ');
        counter = 0;
        double[] ans = new double[str.Length];
        for(int j=0;j<str.Length;j++){
            ans[j] = double.Parse(str[j]);
        }
        return ans;
    }
    public List<int>[] g(int N,int[] f,int[] t){
        List<int>[] ans = new List<int>[N];
        for(int j=0;j<N;j++){
            ans[j] = new List<int>();
        }
        for(int j=0;j<f.Length;j++){
            ans[f[j]].Add(t[j]);
            ans[t[j]].Add(f[j]);
        }
        return ans;
    }
    public List<int>[] g(int N,int M){
        List<int>[] ans = new List<int>[N];
        for(int j=0;j<N;j++){
            ans[j] = new List<int>();
        }
        for(int j=0;j<M;j++){
            int f = i()-1;
            int t = i()-1;
            ans[f].Add(t);
            ans[t].Add(f);
        }
        return ans;
    }
    public List<int>[] g(){
        int N = i();
        int M = i();
        List<int>[] ans = new List<int>[N];
        for(int j=0;j<N;j++){
            ans[j] = new List<int>();
        }
        for(int j=0;j<M;j++){
            int f = i()-1;
            int t = i()-1;
            ans[f].Add(t);
            ans[t].Add(f);
        }
        return ans;
    }
}
public static class Debug{
    public static void Print(double[,,] k){
        for(int i=0;i<k.GetLength(0);i++){
            for(int j=0;j<k.GetLength(1);j++){
                for(int l=0;l<k.GetLength(2);l++){
                    Console.Write(k[i,j,l]+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    public static void Print(double[,] k){
        for(int i=0;i<k.GetLength(0);i++){
            for(int j=0;j<k.GetLength(1);j++){
                Console.Write(k[i,j]+" ");
            }
            Console.WriteLine();
        }
    }
    public static void Print(double[] k){
        for(int i=0;i<k.Length;i++){
            Console.WriteLine(k[i]);
        }
    }
    public static void Print(long[,,] k){
        for(int i=0;i<k.GetLength(0);i++){
            for(int j=0;j<k.GetLength(1);j++){
                for(int l=0;l<k.GetLength(2);l++){
                    Console.Write(k[i,j,l]+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    public static void Print(long[,] k){
        for(int i=0;i<k.GetLength(0);i++){
            for(int j=0;j<k.GetLength(1);j++){
                Console.Write(k[i,j]+" ");
            }
            Console.WriteLine();
        }
    }
    public static void Print(long[] k){
        for(int i=0;i<k.Length;i++){
            Console.WriteLine(k[i]);
        }
    }
    public static void Print(int[,,] k){
        for(int i=0;i<k.GetLength(0);i++){
            for(int j=0;j<k.GetLength(1);j++){
                for(int l=0;l<k.GetLength(2);l++){
                    Console.Write(k[i,j,l]+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
    public static void Print(int[,] k){
        for(int i=0;i<k.GetLength(0);i++){
            for(int j=0;j<k.GetLength(1);j++){
                Console.Write(k[i,j]+" ");
            }
            Console.WriteLine();
        }
    }
    public static void Print(int[] k){
        for(int i=0;i<k.Length;i++){
            Console.WriteLine(k[i]);
        }
    }
}
