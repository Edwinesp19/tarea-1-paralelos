<?php

namespace App\Http\Middleware;

use Closure;
use Exception;
use App\Models\User;
use Firebase\JWT\JWT;
use Firebase\JWT\Key;
use Firebase\JWT\ExpiredException;

class JwtMiddleware
{
    /**
     * Handle an incoming request.
     *
     * @param  \Illuminate\Http\Request  $request
     * @param  \Closure  $next
     * @return mixed
     */
    public function handle($request, Closure $next)
    {
        // Pre-Middleware Action
        if(!$request->header('Authorization')){
            return response()->json([
                'error' => 'Token not provided.'
            ], 401);

        }
        $array_token = explode(" ", $request->header('Authorization'));
        $token = $array_token[1];

        try{
            $credentials = JWT::decode($token,  new Key(env('JWT_SECRET'), 'HS256'));

        }catch(ExpiredException $e){
            return response()->json([
                'error' => 'Provided token is expired.'
            ], 400);
        }catch(Exception $e){
            return response()->json([
                'error' => 'An error while decoding token.'
            ], 400);
        }

        $user = User::find($credentials->sub);
        $request->auth = $user;

        return $next($request);
    }
}
